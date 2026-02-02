import { Route, Routes } from 'react-router-dom';
import Layout from '../components/Layout.jsx';
import ChatPage from '../pages/ChatPage.jsx';
import TriagePage from '../pages/TriagePage.jsx';
import MedicalProfilePage from '../pages/MedicalProfilePage.jsx';
import AppointmentsPage from '../pages/AppointmentsPage.jsx';
import DentistDashboardPage from '../pages/DentistDashboardPage.jsx';
import AdminKbPage from '../pages/AdminKbPage.jsx';
import LoginPage from '../pages/LoginPage.jsx';
import RegisterPage from '../pages/RegisterPage.jsx';

export default function App() {
  return (
    <Routes>
      <Route element={<Layout />}>
        <Route path="/" element={<ChatPage />} />
        <Route path="/triage" element={<TriagePage />} />
        <Route path="/medical-profile" element={<MedicalProfilePage />} />
        <Route path="/appointments" element={<AppointmentsPage />} />
        <Route path="/dentist" element={<DentistDashboardPage />} />
        <Route path="/admin/kb" element={<AdminKbPage />} />
        <Route path="/login" element={<LoginPage />} />
        <Route path="/register" element={<RegisterPage />} />
      </Route>
    </Routes>
  );
}
