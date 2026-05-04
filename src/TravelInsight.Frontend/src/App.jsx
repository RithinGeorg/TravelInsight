import { BrowserRouter, Navigate, Route, Routes } from "react-router-dom";
import { AuthProvider } from "./context/AuthContext.jsx";
import Navbar from "./components/Navbar.jsx";
import ProtectedRoute from "./components/ProtectedRoute.jsx";
import LoginPage from "./pages/LoginPage.jsx";
import DashboardPage from "./pages/DashboardPage.jsx";
import FlightDealsPage from "./pages/FlightDealsPage.jsx";
import CreateFlightDealPage from "./pages/CreateFlightDealPage.jsx";
import EditFlightDealPage from "./pages/EditFlightDealPage.jsx";
import FlightDealDetailsPage from "./pages/FlightDealDetailsPage.jsx";
import DiagnosticsPage from "./pages/DiagnosticsPage.jsx";
import NotFoundPage from "./pages/NotFoundPage.jsx";

export default function App() {
  return (
    <AuthProvider>
      <BrowserRouter>
        <Navbar />
        <main className="container">
          <Routes>
            <Route path="/" element={<Navigate to="/dashboard" />} />
            <Route path="/login" element={<LoginPage />} />
            <Route path="/dashboard" element={<ProtectedRoute><DashboardPage /></ProtectedRoute>} />
            <Route path="/flight-deals" element={<ProtectedRoute><FlightDealsPage /></ProtectedRoute>} />
            <Route path="/flight-deals/create" element={<ProtectedRoute><CreateFlightDealPage /></ProtectedRoute>} />
            <Route path="/flight-deals/:id" element={<ProtectedRoute><FlightDealDetailsPage /></ProtectedRoute>} />
            <Route path="/flight-deals/:id/edit" element={<ProtectedRoute><EditFlightDealPage /></ProtectedRoute>} />
            <Route path="/diagnostics" element={<ProtectedRoute><DiagnosticsPage /></ProtectedRoute>} />
            <Route path="*" element={<NotFoundPage />} />
          </Routes>
        </main>
      </BrowserRouter>
    </AuthProvider>
  );
}
