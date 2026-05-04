import { useState } from "react";
import { useNavigate } from "react-router-dom";
import { useAuth } from "../context/AuthContext.jsx";

export default function LoginPage() {
  const { login } = useAuth();
  const navigate = useNavigate();
  const [email, setEmail] = useState("admin@travelinsight.demo");
  const [password, setPassword] = useState("Admin1234");
  const [error, setError] = useState("");
  const [loading, setLoading] = useState(false);

  async function submit(e) {
    e.preventDefault();
    setLoading(true); setError("");
    try { await login(email, password); navigate("/dashboard"); }
    catch { setError("Login failed. Check backend is running and credentials are correct."); }
    finally { setLoading(false); }
  }

  return <section className="card auth-card"><h1>Login</h1><p>Demo admin is pre-filled.</p>{error && <div className="error">{error}</div>}<form onSubmit={submit}><label>Email</label><input value={email} onChange={e=>setEmail(e.target.value)} /><label>Password</label><input type="password" value={password} onChange={e=>setPassword(e.target.value)} /><button disabled={loading}>{loading ? "Logging in..." : "Login"}</button></form></section>;
}
