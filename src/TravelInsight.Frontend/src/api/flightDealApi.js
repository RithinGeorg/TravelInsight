import apiClient from "./apiClient";

export async function getFlightDeals(params) {
  const response = await apiClient.get("/flightdeals", { params });
  return response.data;
}
export async function getFlightDealById(id) {
  const response = await apiClient.get(`/flightdeals/${id}`);
  return response.data;
}
export async function createFlightDeal(data) {
  const response = await apiClient.post("/flightdeals", data);
  return response.data;
}
export async function updateFlightDeal(id, data) {
  await apiClient.put(`/flightdeals/${id}`, data);
}
export async function deleteFlightDeal(id) {
  await apiClient.delete(`/flightdeals/${id}`);
}
