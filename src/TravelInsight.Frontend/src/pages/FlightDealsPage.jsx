import { useEffect, useState } from "react";
import { Link } from "react-router-dom";
import { deleteFlightDeal, getFlightDeals } from "../api/flightDealApi";
import { useAuth } from "../context/AuthContext.jsx";

export default function FlightDealsPage() {
  const { user } = useAuth();
  const [origin, setOrigin] = useState("");
  const [destination, setDestination] = useState("");
  const [sortBy, setSortBy] = useState("price");
  const [page, setPage] = useState(1);
  const [items, setItems] = useState([]);
  const [total, setTotal] = useState(0);
  const [loading, setLoading] = useState(false);

  async function load() {
    setLoading(true);
    try {
      const result = await getFlightDeals({ origin, destination, sortBy, page, pageSize: 10 });
      setItems(result.items ?? []); setTotal(result.totalCount ?? 0);
    } finally { setLoading(false); }
  }
  useEffect(() => { load(); }, [page, sortBy]);
  async function search(e) { e.preventDefault(); setPage(1); await load(); }
  async function remove(id) { if (confirm("Delete this deal?")) { await deleteFlightDeal(id); await load(); } }

  return <section><h1>Flight Deals</h1><form className="filters" onSubmit={search}><input placeholder="Origin" value={origin} onChange={e=>setOrigin(e.target.value.toUpperCase())}/><input placeholder="Destination" value={destination} onChange={e=>setDestination(e.target.value.toUpperCase())}/><select value={sortBy} onChange={e=>setSortBy(e.target.value)}><option value="price">Price low</option><option value="price_desc">Price high</option><option value="date">Date</option><option value="date_desc">Date desc</option></select><button>Search</button></form>{loading && <p>Loading...</p>}<table><thead><tr><th>Route</th><th>Airline</th><th>Date</th><th>Price</th><th>Seats</th><th>Provider</th><th>Actions</th></tr></thead><tbody>{items.map(d=><tr key={d.id}><td>{d.origin} → {d.destination}</td><td>{d.airline}</td><td>{new Date(d.departureDate).toLocaleDateString()}</td><td>${d.price}</td><td>{d.availableSeats}</td><td>{d.provider}</td><td><Link to={`/flight-deals/${d.id}`}>View</Link> <Link to={`/flight-deals/${d.id}/edit`}>Edit</Link> {user?.role === "Admin" && <button onClick={()=>remove(d.id)}>Delete</button>}</td></tr>)}</tbody></table><div className="pagination"><button disabled={page===1} onClick={()=>setPage(page-1)}>Previous</button><span>Page {page}</span><button disabled={page*10>=total} onClick={()=>setPage(page+1)}>Next</button></div></section>;
}
