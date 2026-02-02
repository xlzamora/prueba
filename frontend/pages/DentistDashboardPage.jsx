import { useState } from 'react';
import { Button, Card, CardContent, Stack, Typography } from '@mui/material';
import api from '../services/api';

export default function DentistDashboardPage() {
  const [appointments, setAppointments] = useState([]);

  const load = async () => {
    const { data } = await api.get('/dentist/appointments');
    setAppointments(data);
  };

  return (
    <Stack spacing={3}>
      <Typography variant="h4">Panel Odontólogo</Typography>
      <Button variant="contained" onClick={load}>Ver citas</Button>
      {appointments.map((appt) => (
        <Card key={appt.appointmentId}>
          <CardContent>
            <Typography variant="h6">Cita {appt.appointmentId}</Typography>
            <Typography>Paciente: {appt.patientId}</Typography>
            <Typography>Prioridad: {appt.priority}</Typography>
            <Typography>Resumen: {appt.summaryText}</Typography>
          </CardContent>
        </Card>
      ))}
    </Stack>
  );
}
