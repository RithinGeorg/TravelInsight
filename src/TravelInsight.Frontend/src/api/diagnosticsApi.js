import apiClient from "./apiClient";

export async function summarizeIncident(logText) {
  const response = await apiClient.post("/diagnostics/ai-incident-summary", { logText });
  return response.data;
}
