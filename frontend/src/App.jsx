import React, { useState, useEffect, useCallback } from 'react';
import { productService } from './services/api';
import FilterPanel from './components/FilterPanel';
import './App.css';

function App() {
  const [products, setProducts] = useState([]);
  const [filteredProducts, setFilteredProducts] = useState([]);
  const [cart, setCart] = useState([]);
  const [loading, setLoading] = useState(true);
  const [user, setUser] = useState(null);
  const [isTelegram, setIsTelegram] = useState(false);
  const [sending, setSending] = useState(false);
  const [showCartModal, setShowCartModal] = useState(false);
  const [filters, setFilters] = useState({
    steelGrade: '',
    diameter: '',
    wallThickness: '',
    manufacturer: ''
  });
  const BACKEND_BASE_URL = 'https://b1b4d0b4249cd8.lhr.life';

  useEffect(() => {
    if (window.Telegram?.WebApp) {
      const tg = window.Telegram.WebApp;
      tg.ready();
      tg.expand();

      const userData = tg.initDataUnsafe?.user;
      if (userData) {
        setUser({
          id: userData.id,
          firstName: userData.first_name,
          lastName: userData.last_name,
          username: userData.username
        });
      }

      setIsTelegram(true);
    }
  }, []);

  useEffect(() => {
    const loadProducts = async () => {
      try {
        const data = await productService.getProducts();
        setProducts(data);
        setFilteredProducts(data);
      } catch (error) {
        console.error('Ошибка загрузки товаров:', error);
      } finally {
        setLoading(false);
      }
    };
    loadProducts();
  }, []);

  // фильтрация товаров при изменении фильтров
  useEffect(() => {
    let filtered = products;

    if (filters.steelGrade) {
      filtered = filtered.filter(product =>
        product.steelGrade === filters.steelGrade
      );
    }

    if (filters.diameter) {
      filtered = filtered.filter(product =>
        product.diameter.toString() === filters.diameter
      );
    }

    if (filters.wallThickness) {
      filtered = filtered.filter(product =>
        product.pipeWallThickness.toString() === filters.wallThickness
      );
    }

    if (filters.manufacturer) {
      filtered = filtered.filter(product =>
        product.manufacturer === filters.manufacturer
      );
    }

    setFilteredProducts(filtered);
  }, [filters, products]);

  useEffect(() => {
    const savedCart = localStorage.getItem('tubeAppCart');
    if (savedCart) setCart(JSON.parse(savedCart));
  }, []);

  useEffect(() => {
    localStorage.setItem('tubeAppCart', JSON.stringify(cart));
  }, [cart]);

  const addToCart = async (product) => {
  try {

    // получаем актуальную цену и остатки
    const [priceData, stockData] = await Promise.all([
      productService.getCurrentPrice(product.id),
      productService.getStock(product.id)
    ]);

    // ищем цену в разных возможных полях
    const price =
      priceData?.priceValue ||
      priceData?.PriceValue ||
      priceData?.price ||
      priceData?.PriceT ||
      priceData?.priceT ||
      priceData?.PriceM ||
      priceData?.priceM ||
      1000;

    // ищем остатки в разных возможных полях
    const totalStock = stockData?.reduce((sum, item) =>
      sum + (item.inStockT || item.InStockT || item.quantity || item.Quantity || 1000), 0) || 1000;

    // проверяем доступность (цена должна быть > 0 и остатки > 0)
    if (price <= 0) {
      alert('Цена не указана для этого товара');
      return;
    }

    if (totalStock <= 0) {
      alert('Товара нет в наличии');
      return;
    }

    const existingItem = cart.find(item => item.id === product.id);
    if (existingItem) {
      setCart(cart.map(item =>
        item.id === product.id ? { ...item, quantity: item.quantity + 1 } : item
      ));
    } else {
      setCart([...cart, {
        ...product,
        price: price,
        quantity: 1,
        unit: 'ton'
      }]);
    }

    if (isTelegram) {
      window.Telegram.WebApp.showPopup({
        title: 'Товар добавлен',
        message: `${product.name}\nЦена: ${price.toLocaleString('ru-RU')} ₽/т`,
        buttons: [{ type: 'ok' }]
      });
    }

  } catch (error) {
    console.error('Error adding to cart:', error);
    alert('Ошибка при добавлении товара в корзину');
  }
};

  const removeFromCart = (productId) => {
    setCart(cart.filter(item => item.id !== productId));
  };

  const updateQuantity = (productId, quantity) => {
    if (quantity <= 0) {
      removeFromCart(productId);
      return;
    }
    setCart(cart.map(item =>
      item.id === productId ? { ...item, quantity } : item
    ));
  };

  const getTotalPrice = useCallback(() => {
    return cart.reduce((total, item) => total + (item.price * item.quantity), 0);
  }, [cart]);

  const handleCheckout = useCallback(async () => {
    if (sending || cart.length === 0) return;

    setSending(true);

    const orderData = {
      user,
      cart: cart.map(item => ({
        nomenclatureId: item.id,
        name: item.name,
        steelGrade: item.steelGrade,
        diameter: item.diameter,
        thickness: item.pipeWallThickness,
        manufacturer: item.manufacturer,
        price: item.price,
        quantity: item.quantity,
        unit: item.unit || 'ton'
      })),
      total: getTotalPrice(),
      timestamp: new Date().toISOString()
    };

    try {
      const response = await fetch(`https://b0c76d971f1adb.lhr.life/webhook`, {
        method: 'POST',
        headers: { 'Content-Type': 'application/json' },
        body: JSON.stringify(orderData)
      });

      const data = await response.json();

      if (!response.ok) {
        throw new Error(data.message || 'Ошибка сервера');
      }

      if (data.success) {
        window.Telegram?.WebApp?.showPopup?.({
          title: 'Заказ принят!',
          message: `Ваш заказ на ${getTotalPrice().toLocaleString('ru-RU')} ₽ успешно оформлен.`,
          buttons: [{ type: 'ok' }]
        });

        setCart([]);
        setShowCartModal(false);
        window.Telegram?.WebApp?.MainButton?.hide();
      } else {
        throw new Error(data.message || 'Ошибка оформления заказа');
      }

    } catch (err) {
      console.error('Ошибка отправки заказа:', err);

      window.Telegram?.WebApp?.showPopup?.({
        title: 'Ошибка',
        message: err.message || 'Ошибка соединения с сервером',
        buttons: [{ type: 'ok' }]
      });
    } finally {
      setSending(false);
    }
  }, [cart, sending, user, getTotalPrice]);

  useEffect(() => {
    if (isTelegram && window.Telegram?.WebApp) {
      const tg = window.Telegram.WebApp;
      tg.MainButton.offClick(handleCheckout);
      tg.MainButton.onClick(handleCheckout);
    }
  }, [isTelegram, handleCheckout]);

  useEffect(() => {
    if (isTelegram && window.Telegram?.WebApp) {
      const tg = window.Telegram.WebApp;
      if (cart.length > 0) {
        tg.MainButton.setText(`Оформить заказ (${getTotalPrice().toLocaleString('ru-RU')} ₽)`);
        tg.MainButton.show();
      } else {
        tg.MainButton.hide();
      }
    }
  }, [cart, isTelegram, getTotalPrice]);

  if (loading) return <div className="loading">Загрузка товаров...</div>;

  return (
    <div className={`app ${isTelegram ? 'telegram-theme' : ''}`}>
      <header className="app-header">
        <h1>Каталог трубной продукции</h1>
        {user && (
          <div className="user-info">
            Клиент: {user.firstName} {user.lastName}<br/>
            Ник: {user.username && `@${user.username}`}
          </div>
        )}

        <div className="cart-controls">
          <button
            onClick={() => setShowCartModal(true)}
            style={{
              background: '#d66b14d7',
              color: 'white',
              border: 'none',
              padding: '8px 15px',
              borderRadius: '5px',
              margin: '5px'
            }}
          >
            Корзина ({cart.length})
          </button>

          {cart.length > 0 && (
            <button
              onClick={() => {
                setCart([]);
                setShowCartModal(false);
              }}
              style={{
                background: '#bc2b39ff',
                color: 'white',
                border: 'none',
                padding: '8px 15px',
                borderRadius: '5px',
                margin: '5px'
              }}
            >
              Очистить корзину
            </button>
          )}
        </div>
      </header>

      {/* панель фильтров */}
      <FilterPanel
        filters={filters}
        onFilterChange={setFilters}
        products={products}
      />

      <div className="products-section">
        <h2>Доступная продукция ({filteredProducts.length} товаров)</h2>
        <div className="products-grid">
          {filteredProducts.length === 0 ? (
            <div className="no-products">
              <p>Товары не найдены</p>
              <button
                onClick={() => setFilters({
                  steelGrade: '',
                  diameter: '',
                  wallThickness: '',
                  manufacturer: ''
                })}
                className="clear-filters-btn"
              >
                Сбросить фильтры
              </button>
            </div>
          ) : (
            filteredProducts.map(product => (
              <div key={product.id} className="product-card">
                <h3>{product.name}</h3>
                <div className="product-specs">
                  <p><strong>Марка стали:</strong> {product.steelGrade || 'Не указано'}</p>
                  <p><strong>Диаметр:</strong> {product.diameter} мм</p>
                  <p><strong>Толщина стенки:</strong> {product.pipeWallThickness} мм</p>
                  <p><strong>Производитель:</strong> {product.manufacturer || 'Не указано'}</p>
                  {product.gost && <p><strong>ГОСТ/ТУ:</strong> {product.gost}</p>}
                </div>
                <button
                  className='add-to-cart-btn'
                  onClick={() => addToCart(product)}
                >
                  Добавить в корзину
                </button>
              </div>
            ))
          )}
        </div>
      </div>

      {/* окно корзины */}
      {showCartModal && (
        <div className="modal-overlay" onClick={() => setShowCartModal(false)}>
          <div className="modal-content" onClick={(e) => e.stopPropagation()}>
            <div className="modal-header">
              <h2>Ваша корзина</h2>
              <button
                className="close-btn"
                onClick={() => setShowCartModal(false)}
              >
                ✕
              </button>
            </div>

            <div className="modal-body">
              {cart.length === 0 ? (
                <div className="empty-cart">
                  <p>Корзина пуста</p>
                  <button
                    onClick={() => setShowCartModal(false)}
                    className="continue-shopping-btn"
                  >
                    Продолжить покупки
                  </button>
                </div>
              ) : (
                <>
                  <div className="cart-items">
                    {cart.map(item => (
                      <div key={item.id} className="cart-item">
                        <div className="item-info">
                          <h4>{item.name}</h4>
                          <p>{item.steelGrade} {item.diameter}мм {item.wallThickness}мм</p>
                          <p className="item-price">{(item.price * item.quantity).toLocaleString('ru-RU')} ₽</p>
                        </div>
                        <div className="item-controls">
                          <div className="quantity-controls">
                            <button onClick={() => updateQuantity(item.id, item.quantity - 1)}>-</button>
                            <span>{item.quantity} шт</span>
                            <button onClick={() => updateQuantity(item.id, item.quantity + 1)}>+</button>
                          </div>
                            <div className="item-total-section">
                                <button
                                onClick={() => removeFromCart(item.id)}
                                className="delete-btn"
                                title="Удалить"
                                >
                                🗑️
                                </button>
                            </div>
                        </div>
                      </div>
                    ))}
                  </div>
                </>
              )}
            </div>
          </div>
        </div>
      )}
    </div>
  );
}

export default App;
