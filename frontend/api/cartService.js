import axios from "axios";
import { SERVER_URL } from "./config";

export const cartService = {
  getCart: async (userId) => {
    return axios.get(`${SERVER_URL}/cart/${userId}`);
  },
  addToCart: async (userId, productId) => {
    return axios.post(`${SERVER_URL}/cart/${userId}`, { productId });
  },
  removeFromCart: async (userId, productId) => {
    return axios.delete(`${SERVER_URL}/cart/${userId}/${productId}`);
  },
};
