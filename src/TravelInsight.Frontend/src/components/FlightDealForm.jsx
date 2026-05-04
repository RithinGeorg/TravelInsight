export default function FlightDealForm({ form, setForm, onSubmit, submitText, error, loading }) {
  function update(field, value) {
    setForm({ ...form, [field]: value });
  }

  return (
    <form className="card form" onSubmit={onSubmit}>
      {error && <div className="error">{error}</div>}
      <label>Origin airport code</label>
      <input value={form.origin} onChange={e => update("origin", e.target.value.toUpperCase())} maxLength="3" />
      <label>Destination airport code</label>
      <input value={form.destination} onChange={e => update("destination", e.target.value.toUpperCase())} maxLength="3" />
      <label>Departure date</label>
      <input type="date" value={form.departureDate} onChange={e => update("departureDate", e.target.value)} />
      <label>Price</label>
      <input type="number" value={form.price} onChange={e => update("price", Number(e.target.value))} />
      <label>Airline</label>
      <input value={form.airline} onChange={e => update("airline", e.target.value)} />
      <label>Available seats</label>
      <input type="number" value={form.availableSeats} onChange={e => update("availableSeats", Number(e.target.value))} />
      <label>Provider</label>
      <input value={form.provider} onChange={e => update("provider", e.target.value)} />
      <button disabled={loading}>{loading ? "Saving..." : submitText}</button>
    </form>
  );
}
