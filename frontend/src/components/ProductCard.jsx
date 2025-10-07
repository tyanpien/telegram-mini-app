import React, { useState, useEffect } from 'react';
import { productService } from '../services/api';
import './ProductCard.css';

const ProductCard = ({ product, onAddToCart }) => {
  const [currentPrice, setCurrentPrice] = useState(null);
  const [stock, setStock] = useState(0);
  const [loading, setLoading] = useState(true);

  useEffect(() => {
    loadProductDetails();
  }, [product.id]);

  const loadProductDetails = async () => {
    try {
      const [priceData, stockData] = await Promise.all([
        productService.getCurrentPrice(product.id),
        productService.getStock(product.id)
      ]);

      // обработка данных о цене
      if (priceData) {
        setCurrentPrice(priceData.priceValue || priceData.priceWithNDS || 0);
      } else {
        setCurrentPrice(0);
      }

      // обработка данных об остатках
      if (stockData && stockData.length > 0) {
        // суммируем остатки в тоннах из всех складов
        const totalStock = stockData.reduce((sum, item) => sum + (item.inStockT || 0), 0);
        setStock(totalStock);
      } else {
        setStock(0);
      }
    } catch (error) {
      console.error('Error loading product details:', error);
      setCurrentPrice(0);
      setStock(0);
    } finally {
      setLoading(false);
    }
  };

  const handleAddToCart = () => {
    if (currentPrice && currentPrice > 0 && stock > 0) {
      onAddToCart({
        id: product.id,
        name: product.name,
        steelGrade: product.steelGrade,
        diameter: product.diameter,
        thickness: product.pipeWallThickness,
        manufacturer: product.manufacturer,
        price: currentPrice,
        quantity: 1,
        unit: 'ton'
      });
    }
  };

  if (loading) {
    return <div className="product-card loading">Загрузка...</div>;
  }

  const isAvailable = currentPrice && currentPrice > 0 && stock > 0;

  return (
    <div className="product-card">
      <div className="product-info">
        <h3 className="product-name">{product.name}</h3>

        <div className="product-specs">
          <div className="spec">
            <span className="spec-label">Марка стали:</span>
            <span className="spec-value">{product.steelGrade || 'Не указано'}</span>
          </div>

          <div className="spec">
            <span className="spec-label">Диаметр:</span>
            <span className="spec-value">{product.diameter} мм</span>
          </div>

          <div className="spec">
            <span className="spec-label">Толщина стенки:</span>
            <span className="spec-value">{product.pipeWallThickness} мм</span>
          </div>

          <div className="spec">
            <span className="spec-label">Производитель:</span>
            <span className="spec-value">{product.manufacturer || 'Не указано'}</span>
          </div>

          <div className="spec">
            <span className="spec-label">ГОСТ/ТУ:</span>
            <span className="spec-value">{product.gost || 'Не указано'}</span>
          </div>
        </div>

        <div className="product-pricing">
          <div className="price">
            {currentPrice && currentPrice > 0
              ? `${currentPrice.toLocaleString('ru-RU')} ₽/т`
              : 'Цена не указана'
            }
          </div>
          <div className={`stock ${stock > 0 ? 'in-stock' : 'out-of-stock'}`}>
            {stock > 0 ? `На складе: ${stock.toFixed(1)} шт` : 'Нет в наличии'}
          </div>
        </div>
      </div>

      <button
        className={`add-to-cart-btn ${isAvailable ? 'available' : 'unavailable'}`}
        onClick={handleAddToCart}
        disabled={!isAvailable}
      >
        {isAvailable ? 'Добавить в корзину' : 'Нет в наличии'}
      </button>
    </div>
  );
};

export default ProductCard;
