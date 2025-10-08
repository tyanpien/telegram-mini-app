import axios from 'axios';

const API_BASE_URL = 'https://4f218293d711e7.lhr.life/api';

const api = axios.create({
  baseURL: API_BASE_URL,
  timeout: 10000,
});

export const productService = {
  // получить все товары
  getProducts: async (filters = {}) => {
    try {
      const response = await api.get('/nomenclature');
      return response.data;
    } catch (error) {
      console.error('Ошибка загрузки товаров', error);
      return mockProducts;
    }
  },

  getCurrentPrice: async (productId) => {
    try {
      let priceData;

      try {
        // endpoint для лучшей цены за тонну
        const response = await api.get(`/price/best/ton/${productId}`);
        const priceValue = response.data;

        if (priceValue > 0) {
          priceData = { PriceT: priceValue };
        } else {
          throw new Error('Цена равна 0');
        }
      } catch (error1) {
        const mockPriceData = {
          "10001": { PriceT: 28500 },
          "10002": { PriceT: 32400 },
          "10003": { PriceT: 26700 }
        };

        priceData = mockPriceData[productId] || { PriceT: 25000 };
      }

      return priceData;

    } catch (error) {
      console.error(`Ошибка получения цены`, error);
      return { PriceT: 25000 };
    }
  },

  getStock: async (productId) => {
    try {
      const response = await api.get(`/remnant/nomenclature/${productId}`);

      // если остатки пустые, используем mock данные
      let stockData = response.data || [];

      // если данные в формате { ArrayOfRemnantsEl: [...] }, извлекаем массив
      if (stockData && stockData.ArrayOfRemnantsEl) {
        stockData = stockData.ArrayOfRemnantsEl;
      }

      // если нет остатков, используем mock
      if (!stockData || stockData.length === 0) {
        const mockStockData = {
          "10001": [{ InStockT: 125, Warehouse: "Основной склад" }],
          "10002": [{ InStockT: 80, Warehouse: "Склад ВТЗ" }],
          "10003": [{ InStockT: 210, Warehouse: "Склад ЧТПЗ" }]
        };
        stockData = mockStockData[productId] || [{ InStockT: 100, Warehouse: "Основной склад" }];
      }

      return stockData;

    } catch (error) {
      console.error(`Ошибка получения остатков`, error);
      return [{ InStockT: 100, Warehouse: "Основной склад" }];
    }
  },

  searchProducts: async (searchTerm) => {
    try {
      const response = await api.get(`/nomenclature/search?searchTerm=${searchTerm}`);
      return response.data;
    } catch (error) {
      console.error('Error searching products:', error);
      return [];
    }
  },

  getProductById: async (id) => {
    try {
      const response = await api.get(`/nomenclature/${id}`);
      return response.data;
    } catch (error) {
      console.error('Error fetching product by id:', error);
      return null;
    }
  },

  getProductTypes: async () => {
    try {
      const response = await api.get('/type');
      return response.data;
    } catch (error) {
      console.error('Error fetching product types:', error);
      return [];
    }
  }
};
