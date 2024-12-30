import axios from "axios";
import { SERVER_URL } from "./config";

export const authService = {
  login: async (email, password) => {
    return axios.post(
      `${SERVER_URL}/authentification/login?email=${encodeURIComponent(
        email
      )}&password=${encodeURIComponent(password)}`
    );
  },
};
