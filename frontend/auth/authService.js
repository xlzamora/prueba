import api from '../services/api';

export async function login(payload) {
  const { data } = await api.post('/auth/login', payload);
  localStorage.setItem('token', data.token);
  return data;
}

export async function registerPatient(payload) {
  const { data } = await api.post('/auth/register-patient', payload);
  localStorage.setItem('token', data.token);
  return data;
}

export async function getCurrentUser() {
  const { data } = await api.get('/auth/me');
  return data;
}
