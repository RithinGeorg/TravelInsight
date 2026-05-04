import { useEffect, useState } from "react";
import { useNavigate, useParams } from "react-router-dom";
import FlightDealForm from "../components/FlightDealForm.jsx";
import { getFlightDealById, updateFlightDeal } from "../api/flightDealApi";

export default function EditFlightDealPage() {
  const { id } = useParams();
  const navigate = useNavigate();
  const [form, setForm] = useState(null);
  const [error, setError] = useState("");
  const [loading, setLoading] = useState(false);
  useEffect(() => { (async()=>{ const d = await getFlightDealById(id); setForm({ ...d, departureDate: d.departureDate.slice(0,10) }); })(); }, [id]);
  async function submit(e) { e.preventDefault(); setLoading(true); setError(""); try { await updateFlightDeal(id, form); navigate(`/flight-deals/${id}`); } catch(err) { setError(err.response?.data?.error ?? "Update failed."); } finally { setLoading(false); } }
  if (!form) return <p>Loading deal...</p>;
  return <section><h1>Edit Flight Deal</h1><FlightDealForm form={form} setForm={setForm} onSubmit={submit} submitText="Save Changes" error={error} loading={loading} /></section>;
}
