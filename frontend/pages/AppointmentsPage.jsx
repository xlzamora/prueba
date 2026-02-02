import { useState } from 'react';
import { Button, Card, CardContent, Stack, TextField, Typography } from '@mui/material';
import api from '../services/api';

export default function AppointmentsPage() {
  const [dentistId, setDentistId] = useState('');
  const [date, setDate] = useState('');
  const [slots, setSlots] = useState([]);
  const [appointment, setAppointment] = useState({
    patientId: '',
    clinicId: '',
    serviceId: '',
    priority: 'Media',
    summaryText: '',
  });
  const [created, setCreated] = useState(null);

  const loadSlots = async () => {
    const { data } = await api.get('/appointments/available', {
      params: { dentistId, date },
    });
    setSlots(data);
  };

  const createAppointment = async (slot) => {
    const { data } = await api.post('/appointments', {
      patientId: appointment.patientId,
      dentistId,
      clinicId: appointment.clinicId,
      serviceId: appointment.serviceId,
      startAt: slot.startAt,
      endAt: slot.endAt,
      priority: appointment.priority,
      triageId: null,
      summaryText: appointment.summaryText,
    });
    setCreated(data);
  };

  return (
    <Stack spacing={3}>
      <Typography variant="h4">Agenda</Typography>
      <Card>
        <CardContent>
          <Stack spacing={2}>
            <TextField label="DentistId" value={dentistId} onChange={(e) => setDentistId(e.target.value)} />
            <TextField
              label="Fecha (YYYY-MM-DD)"
              value={date}
              onChange={(e) => setDate(e.target.value)}
            />
            <Button variant="contained" onClick={loadSlots}>Ver 3 horarios</Button>
          </Stack>
        </CardContent>
      </Card>
      <Card>
        <CardContent>
          <Stack spacing={2}>
            <TextField label="PatientId" value={appointment.patientId} onChange={(e) => setAppointment({ ...appointment, patientId: e.target.value })} />
            <TextField label="ClinicId" value={appointment.clinicId} onChange={(e) => setAppointment({ ...appointment, clinicId: e.target.value })} />
            <TextField label="ServiceId" value={appointment.serviceId} onChange={(e) => setAppointment({ ...appointment, serviceId: e.target.value })} />
            <TextField label="Resumen" value={appointment.summaryText} onChange={(e) => setAppointment({ ...appointment, summaryText: e.target.value })} />
            {slots.map((slot) => (
              <Button key={slot.startAt} variant="outlined" onClick={() => createAppointment(slot)}>
                {slot.startAt} - {slot.endAt}
              </Button>
            ))}
          </Stack>
        </CardContent>
      </Card>
      {created && (
        <Card>
          <CardContent>
            <Typography variant="h6">Cita creada</Typography>
            <Typography>ID: {created.appointmentId}</Typography>
          </CardContent>
        </Card>
      )}
    </Stack>
  );
}
