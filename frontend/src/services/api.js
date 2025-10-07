import axios from 'axios';

const API_BASE_URL = 'https://e66e79c4e4483e0b157a0685a6664a08.serveo.net/api';

const api = axios.create({
  baseURL: API_BASE_URL,
});

export const productService = {
  // получить все товары
  getProducts: async (filters = {}) => {
    try {
      const response = await api.get('/nomenclature');
      return response.data;
    } catch (error) {
      console.error('Error fetching products:', error);
      return [];
    }
  },

  // поиск товаров
  searchProducts: async (searchTerm) => {
    try {
      const response = await api.get(`/nomenclature/search?searchTerm=${searchTerm}`);
      return response.data;
    } catch (error) {
      console.error('Error searching products:', error);
      return [];
    }
  },

  // получить товар по ID
  getProductById: async (id) => {
    try {
      const response = await api.get(`/nomenclature/${id}`);
      return response.data;
    } catch (error) {
      console.error('Error fetching product by id:', error);
      return null;
    }
  },

  // получить остатки по товару
  getStock: async (productId) => {
    try {
      const response = await api.get(`/remnant/nomenclature/${productId}`);
      return response.data;
    } catch (error) {
      console.error('Error fetching stock:', error);
      return [];
    }
  },

  // получить текущую цену для товара
  getCurrentPrice: async (productId) => {
    try {
      const response = await api.get(`/price/best?nomenclatureId=${productId}&quantity=1&unit=ton`);
      return response.data;
    } catch (error) {
      console.error('Error fetching price:', error);
      return null;
    }
  },

  // получить типы товаров
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

export const cartService = {
  calculateDiscount: (quantity, basePrice) => {
    if (quantity >= 1000) return 0.15;
    if (quantity >= 500) return 0.10;
    if (quantity >= 100) return 0.05;
    return 0;
  },

  calculateTotal: (items) => {
    return items.reduce((total, item) => {
      const discount = cartService.calculateDiscount(item.quantity, item.price);
      const discountedPrice = item.price * (1 - discount);
      return total + (discountedPrice * item.quantity);
    }, 0);
  }
};
