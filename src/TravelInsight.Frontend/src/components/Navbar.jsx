import { Link, useNavigate } from "react-router-dom";
import { useAuth } from "../context/AuthContext.jsx";

export default function Navbar() {
  const { isAuthenticated, user, logout } = useAuth();
  const navigate = useNavigate();

  function handleLogout() {
    logout();
    navigate("/login");
  }

  return (
    <nav className="navbar">
      <Link className="brand" to="/dashboard">TravelInsight Pro</Link>
      {isAuthenticated && <div className="nav-links">
        <Link to="/dashboard">Dashboard</Link>
        <Link to="/flight-deals">Flight Deals</Link>
        <Link to="/flight-deals/create">Create</Link>
        <Link to="/diagnostics">AI Diagnostics</Link>
        <span>{user?.displayName} ({user?.role})</span>
        <button onClick={handleLogout}>Logout</button>
      </div>}
    </nav>
  );
}
