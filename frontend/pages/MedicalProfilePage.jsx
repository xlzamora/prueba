import { useState } from 'react';
import { Button, Card, CardContent, Stack, TextField, Typography } from '@mui/material';
import api from '../services/api';

export default function MedicalProfilePage() {
  const [patientId, setPatientId] = useState('');
  const [bloodType, setBloodType] = useState('');
  const [allergies, setAllergies] = useState('');
  const [conditions, setConditions] = useState('');
  const [saved, setSaved] = useState(null);

  const save = async () => {
    const { data } = await api.post(`/patients/${patientId}/medical-profile`, {
      bloodType,
      allergiesText: allergies,
      conditionsText: conditions,
    });
    setSaved(data);
  };

  return (
    <Stack spacing={3}>
      <Typography variant="h4">Ficha Médica</Typography>
      <Card>
        <CardContent>
          <Stack spacing={2}>
            <TextField label="PatientId" value={patientId} onChange={(e) => setPatientId(e.target.value)} />
            <TextField label="Tipo de sangre" value={bloodType} onChange={(e) => setBloodType(e.target.value)} />
            <TextField
              label="Alergias"
              value={allergies}
              onChange={(e) => setAllergies(e.target.value)}
              multiline
              rows={3}
            />
            <TextField
              label="Condiciones"
              value={conditions}
              onChange={(e) => setConditions(e.target.value)}
              multiline
              rows={3}
            />
            <Button variant="contained" onClick={save}>Guardar</Button>
          </Stack>
        </CardContent>
      </Card>
      {saved && (
        <Card>
          <CardContent>
            <Typography variant="h6">Actualizado</Typography>
            <Typography>BloodType: {saved.bloodType}</Typography>
            <Typography>UpdatedAt: {saved.updatedAt}</Typography>
          </CardContent>
        </Card>
      )}
    </Stack>
  );
}
