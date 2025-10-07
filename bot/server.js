const express = require('express');
const TelegramBot = require('node-telegram-bot-api');
const cors = require('cors');

const app = express();
app.use(express.json());
app.use(cors());

const TOKEN = '8063838686:AAHF0GgPiDeUx83UgdgYNIsvUwhZN2-bC9E';
const bot = new TelegramBot(TOKEN, { polling: true });
const ADMIN_CHAT_ID = '751712580';
const recentOrders = new Set();

bot.onText(/\/start/, (msg) => {
  const chatId = msg.chat.id;
  const welcomeText = `Добро пожаловать в магазин трубной продукции!

Здесь вы можете:
• Просмотреть каталог продукции
• Оформить заказ прямо в Telegram

Нажмите кнопку ниже чтобы открыть каталог:`;

  bot.sendMessage(chatId, welcomeText, {
    reply_markup: {
      inline_keyboard: [[
        {
          text: 'Открыть каталог',
          web_app: { url: 'https://bf64d7981befb0e8c049b83b93872c1c.serveo.net' }
        }
      ]]
    }
  });
});

app.post('/webhook', async (req, res) => {

  try {
    const { user, cart, total } = req.body;

    setTimeout(() => recentOrders.delete(orderKey), 10000);

    let orderText = `НОВЫЙ ЗАКАЗ!\n\n`;

    if (user) {
      orderText += `Клиент: ${user.firstName || ''} ${user.lastName || ''}\n`;
      if (user.username) orderText += `@${user.username}\n`;
    }

    orderText += `\n`;

    let calculatedTotal = 0;

    if (cart && Array.isArray(cart) && cart.length > 0) {
      orderText += `Состав заказа (${cart.length}):\n\n`;

      cart.forEach((item, index) => {
        const quantity = Number(item.quantity) || 0;
        const price = Number(item.price) || 0;
        const itemTotal = quantity * price;
        calculatedTotal += itemTotal;

        orderText += `${index + 1}. ${item.name || 'Товар'}\n`;
        orderText += `Марка: ${item.steelGrade || '-'}\n`;
        orderText += `Диаметр: ⌀${item.diameter || '?'}мм\n`;
        orderText += `Толщина: ${item.thickness || item.wallThickness || '?'}мм\n`;
        orderText += `Количество: ${quantity} т\n`;
      });
    } else {
      calculatedTotal = Number(total) || 0;
      orderText += `Корзина пуста\n\n`;
    }

    orderText += `ОБЩАЯ СУММА: ${calculatedTotal.toLocaleString('ru-RU')} ₽\n`;
    orderText += `${new Date().toLocaleString('ru-RU')}`;


    await bot.sendMessage(ADMIN_CHAT_ID, orderText);

    res.json({
      success: true,
      message: `Заказ на ${calculatedTotal.toLocaleString('ru-RU')}₽ принят!`
    });

  } catch (error) {
    console.error('Ошибка:', error);

    res.status(500).json({
      success: false,
      error: 'Ошибка сервера'
    });
  }
});

const PORT = 3001;
app.listen(PORT, () => {
  console.log(`Бот работает`);
});
