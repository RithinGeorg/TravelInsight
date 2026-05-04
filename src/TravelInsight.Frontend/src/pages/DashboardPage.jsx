import { useEffect, useState } from "react";
import { getFlightDeals } from "../api/flightDealApi";

export default function DashboardPage() {
  const [summary, setSummary] = useState({ total: 0, cheapest: 0, soldOut: 0, routes: 0 });

  useEffect(() => { (async () => {
    const result = await getFlightDeals({ page: 1, pageSize: 100 });
    const items = result.items ?? [];
    setSummary({
      total: result.totalCount ?? items.length,
      cheapest: items.length ? Math.min(...items.map(x => x.price)) : 0,
      soldOut: items.filter(x => x.availableSeats === 0).length,
      routes: new Set(items.map(x => `${x.origin}-${x.destination}`)).size
    });
  })(); }, []);

  return <section><h1>Dashboard</h1><p>Business summary view for product and operations teams.</p><div className="grid"><div className="card"><h3>Total Deals</h3><strong>{summary.total}</strong></div><div className="card"><h3>Cheapest Deal</h3><strong>${summary.cheapest}</strong></div><div className="card"><h3>Sold Out</h3><strong>{summary.soldOut}</strong></div><div className="card"><h3>Active Routes</h3><strong>{summary.routes}</strong></div></div></section>;
}
