import React from 'react';
import { useTelegram } from '../context/TelegramContext';
import './Cart.css';

const Cart = ({ cart, onUpdateQuantity, onRemoveItem }) => {
  const { user, sendData } = useTelegram();

  const getTotalPrice = () => {
    return cart.reduce((total, item) => total + (item.price * item.quantity), 0);
  };

  const handleCheckout = () => {
    const orderData = {
      user: {
        id: user?.id,
        firstName: user?.first_name,
        lastName: user?.last_name,
        username: user?.username
      },
      cart: cart.map(item => ({
        nomenclatureId: item.id,
        name: item.name,
        steelGrade: item.steelGrade,
        diameter: item.diameter,
        thickness: item.thickness,
        manufacturer: item.manufacturer,
        price: item.price,
        quantity: item.quantity,
        unit: item.unit || 'ton'
      })),
      total: getTotalPrice(),
      timestamp: new Date().toISOString()
    };

    sendData(orderData);
  };

  if (cart.length === 0) {
    return (
      <div className="cart-empty">
        <h3>Корзина пуста</h3>
        <p>Добавьте товары из каталога</p>
      </div>
    );
  }

  return (
    <div className="cart">
      <h2>Ваша корзина</h2>

      <div className="cart-items">
        {cart.map(item => (
          <div key={item.id} className="cart-item">
            <div className="item-info">
              <h4>{item.name}</h4>
              <p>{item.steelGrade} · ⌀{item.diameter}мм · {item.thickness}мм</p>
              <p className="item-manufacturer">{item.manufacturer}</p>
              <p className="item-price">{item.price.toLocaleString('ru-RU')} ₽/т</p>
            </div>

            <div className="item-controls">
              <div className="quantity-controls">
                <button
                  onClick={() => onUpdateQuantity(item.id, Math.max(0, item.quantity - 1))}
                  disabled={item.quantity <= 1}
                >
                  -
                </button>
                <span>{item.quantity} т</span>
                <button onClick={() => onUpdateQuantity(item.id, item.quantity + 1)}>+</button>
              </div>

              <div className="item-total">
                {(item.price * item.quantity).toLocaleString('ru-RU')} ₽
              </div>

              <button
                onClick={() => onRemoveItem(item.id)}
                className="remove-btn"
              >
                Удалить
              </button>
            </div>
          </div>
        ))}
      </div>

      <div className="cart-total">
        <h3>Итого: {getTotalPrice().toLocaleString('ru-RU')} ₽</h3>

        <button className="checkout-btn" onClick={handleCheckout}>
          Оформить заказ
        </button>
      </div>
    </div>
  );
};

export default Cart;
