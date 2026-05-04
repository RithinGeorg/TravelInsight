import { useEffect, useState } from "react";
import { Link, useParams } from "react-router-dom";
import { getFlightDealById } from "../api/flightDealApi";

export default function FlightDealDetailsPage() {
  const { id } = useParams();
  const [deal, setDeal] = useState(null);
  useEffect(() => { getFlightDealById(id).then(setDeal); }, [id]);
  if (!deal) return <p>Loading...</p>;
  return <section className="card"><h1>{deal.origin} → {deal.destination}</h1><p><b>Airline:</b> {deal.airline}</p><p><b>Date:</b> {new Date(deal.departureDate).toLocaleDateString()}</p><p><b>Price:</b> ${deal.price}</p><p><b>Seats:</b> {deal.availableSeats}</p><p><b>Provider:</b> {deal.provider}</p><Link to={`/flight-deals/${deal.id}/edit`}>Edit this deal</Link></section>;
}
