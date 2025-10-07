import React, { useState, useEffect } from 'react';
import { productService } from '../services/api';
import ProductCard from '../components/ProductCard';
import FilterPanel from '../components/FilterPanel';
import Cart from '../components/Cart';
import './ProductCatalog.css';

const ProductCatalog = () => {
  const [products, setProducts] = useState([]);
  const [filteredProducts, setFilteredProducts] = useState([]);
  const [cart, setCart] = useState([]);
  const [filters, setFilters] = useState({
    steelGrade: '',
    diameter: '',
    wallThickness: '',
    manufacturer: ''
  });
  const [loading, setLoading] = useState(true);

  useEffect(() => {
    loadProducts();
    loadCartFromStorage();
  }, []);

  useEffect(() => {
    filterProducts();
  }, [products, filters]);

  const loadProducts = async () => {
    try {
      setLoading(true);
      const data = await productService.getProducts();
      setProducts(data);
    } catch (error) {
      console.error('Error loading products:', error);
    } finally {
      setLoading(false);
    }
  };

  const loadCartFromStorage = () => {
    const savedCart = localStorage.getItem('tubeCart');
    if (savedCart) {
      setCart(JSON.parse(savedCart));
    }
  };

  const saveCartToStorage = (cart) => {
    localStorage.setItem('tubeCart', JSON.stringify(cart));
  };

  const filterProducts = () => {
    let filtered = products;
    if (filters.steelGrade) {
      filtered = filtered.filter(p =>
        p.steelGrade?.toLowerCase().includes(filters.steelGrade.toLowerCase())
      );
    }
    if (filters.diameter) {
      filtered = filtered.filter(p => p.diameter == filters.diameter);
    }
    if (filters.wallThickness) {
      filtered = filtered.filter(p => p.pipeWallThickness == filters.wallThickness);
    }
    if (filters.manufacturer) {
      filtered = filtered.filter(p =>
        p.manufacturer?.toLowerCase().includes(filters.manufacturer.toLowerCase())
      );
    }
    setFilteredProducts(filtered);
  };

  const addToCart = (product) => {
    const existingItem = cart.find(item => item.id === product.id);

    if (existingItem) {
      const updatedCart = cart.map(item =>
        item.id === product.id
          ? { ...item, quantity: item.quantity + 1 }
          : item
      );
      setCart(updatedCart);
      saveCartToStorage(updatedCart);
    } else {
      const updatedCart = [...cart, product];
      setCart(updatedCart);
      saveCartToStorage(updatedCart);
    }
  };

  const updateQuantity = (productId, newQuantity) => {
    if (newQuantity <= 0) {
      removeFromCart(productId);
      return;
    }

    const updatedCart = cart.map(item =>
      item.id === productId
        ? { ...item, quantity: newQuantity }
        : item
    );
    setCart(updatedCart);
    saveCartToStorage(updatedCart);
  };

  const removeFromCart = (productId) => {
    const updatedCart = cart.filter(item => item.id !== productId);
    setCart(updatedCart);
    saveCartToStorage(updatedCart);
  };

  if (loading) {
    return <div className="loading">Загрузка продукции...</div>;
  }

  return (
    <div className="product-catalog">
      <h1>Каталог трубной продукции</h1>

      <FilterPanel
        filters={filters}
        onFilterChange={setFilters}
        products={products}
      />

      <div className="catalog-layout">
        <div className="products-section">
          <div className="products-grid">
            {filteredProducts.map(product => (
              <ProductCard
                key={product.id}
                product={product}
                onAddToCart={addToCart}
              />
            ))}
          </div>

          {filteredProducts.length === 0 && (
            <div className="no-products">
              Продукция не найдена. Попробуйте изменить фильтры.
            </div>
          )}
        </div>

        <div className="cart-section">
          <Cart
            cart={cart}
            onUpdateQuantity={updateQuantity}
            onRemoveItem={removeFromCart}
          />
        </div>
      </div>
    </div>
  );
};

export default ProductCatalog;
