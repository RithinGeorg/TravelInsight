import { useState } from "react";
import { useNavigate } from "react-router-dom";
import FlightDealForm from "../components/FlightDealForm.jsx";
import { createFlightDeal } from "../api/flightDealApi";

export default function CreateFlightDealPage() {
  const navigate = useNavigate();
  const [form, setForm] = useState({
    origin: "OOL", destination: "SYD", departureDate: new Date(Date.now()+86400000*14).toISOString().slice(0,10), price: 199, airline: "Jetstar", availableSeats: 20, provider: "Skyscanner"
  });
  const [error, setError] = useState("");
  const [loading, setLoading] = useState(false);
  async function submit(e) { e.preventDefault(); setLoading(true); setError(""); try { const created = await createFlightDeal(form); navigate(`/flight-deals/${created.id}`); } catch (err) { setError(err.response?.data?.error ?? "Create failed."); } finally { setLoading(false); } }
  return <section><h1>Create Flight Deal</h1><FlightDealForm form={form} setForm={setForm} onSubmit={submit} submitText="Create Deal" error={error} loading={loading} /></section>;
}
