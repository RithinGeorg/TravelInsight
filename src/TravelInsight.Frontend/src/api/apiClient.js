import axios from "axios";

const apiClient = axios.create({
  baseURL: "https://localhost:5001/api",
  withCredentials: true
});

// Senior point: one interceptor avoids repeating Authorization header code in every component.
apiClient.interceptors.request.use((config) => {
  const token = localStorage.getItem("accessToken");
  if (token) config.headers.Authorization = `Bearer ${token}`;
  return config;
});

export default apiClient;
