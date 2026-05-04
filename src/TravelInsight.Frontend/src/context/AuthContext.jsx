import { createContext, useContext, useState } from "react";
import { loginUser } from "../api/authApi";

const AuthContext = createContext(null);

export function AuthProvider({ children }) {
  const [token, setToken] = useState(() => localStorage.getItem("accessToken"));
  const [user, setUser] = useState(() => {
    const raw = localStorage.getItem("user");
    return raw ? JSON.parse(raw) : null;
  });

  async function login(email, password) {
    const result = await loginUser(email, password);
    localStorage.setItem("accessToken", result.accessToken);
    localStorage.setItem("user", JSON.stringify(result.user));
    setToken(result.accessToken);
    setUser(result.user);
  }

  function logout() {
    localStorage.removeItem("accessToken");
    localStorage.removeItem("user");
    setToken(null);
    setUser(null);
  }

  return <AuthContext.Provider value={{ token, user, login, logout, isAuthenticated: Boolean(token) }}>{children}</AuthContext.Provider>;
}

export function useAuth() {
  return useContext(AuthContext);
}
