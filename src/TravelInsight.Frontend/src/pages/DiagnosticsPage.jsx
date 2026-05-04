import { useState } from "react";
import { summarizeIncident } from "../api/diagnosticsApi";

export default function DiagnosticsPage() {
  const [logText, setLogText] = useState("SQL timeout while searching flight deals");
  const [result, setResult] = useState(null);
  async function submit(e) { e.preventDefault(); setResult(await summarizeIncident(logText)); }
  return <section><h1>AI Diagnostics</h1><p>Paste log text and get a demo root-cause summary.</p><form className="card form" onSubmit={submit}><textarea rows="8" value={logText} onChange={e=>setLogText(e.target.value)} /><button>Analyze Logs</button></form>{result && <div className="card"><h3>{result.category}</h3><p>{result.summary}</p><strong>Suggested action</strong><p>{result.suggestedAction}</p></div>}</section>;
}
