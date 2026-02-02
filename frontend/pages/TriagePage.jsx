import { useState } from 'react';
import { Button, Card, CardContent, FormControl, InputLabel, MenuItem, Select, Stack, Switch, TextField, Typography } from '@mui/material';
import api from '../services/api';

export default function TriagePage() {
  const [sessionId, setSessionId] = useState('');
  const [patientId, setPatientId] = useState('');
  const [mainComplaint, setMainComplaint] = useState('Dolor');
  const [painLevel, setPainLevel] = useState('moderado');
  const [swelling, setSwelling] = useState(false);
  const [result, setResult] = useState(null);

  const submit = async () => {
    const { data } = await api.post('/triage', {
      sessionId,
      patientId,
      mainComplaint,
      painLevel,
      swelling,
      answers: { painLevel, swelling: swelling.toString() },
    });
    setResult(data);
  };

  return (
    <Stack spacing={3}>
      <Typography variant="h4">Triage</Typography>
      <Card>
        <CardContent>
          <Stack spacing={2}>
            <TextField label="SessionId" value={sessionId} onChange={(e) => setSessionId(e.target.value)} />
            <TextField label="PatientId" value={patientId} onChange={(e) => setPatientId(e.target.value)} />
            <FormControl fullWidth>
              <InputLabel>Motivo principal</InputLabel>
              <Select value={mainComplaint} label="Motivo principal" onChange={(e) => setMainComplaint(e.target.value)}>
                <MenuItem value="Dolor">Dolor</MenuItem>
                <MenuItem value="Encías">Encías</MenuItem>
                <MenuItem value="Trauma">Trauma</MenuItem>
              </Select>
            </FormControl>
            <FormControl fullWidth>
              <InputLabel>Nivel de dolor</InputLabel>
              <Select value={painLevel} label="Nivel de dolor" onChange={(e) => setPainLevel(e.target.value)}>
                <MenuItem value="leve">Leve</MenuItem>
                <MenuItem value="moderado">Moderado</MenuItem>
                <MenuItem value="fuerte">Fuerte</MenuItem>
              </Select>
            </FormControl>
            <Stack direction="row" spacing={2} alignItems="center">
              <Typography>Hinchazón</Typography>
              <Switch checked={swelling} onChange={(e) => setSwelling(e.target.checked)} />
            </Stack>
            <Button variant="contained" onClick={submit}>Evaluar</Button>
          </Stack>
        </CardContent>
      </Card>
      {result && (
        <Card>
          <CardContent>
            <Typography variant="h6">Resultado</Typography>
            <Typography>Prioridad: {result.priority}</Typography>
            <Typography>Resumen: {result.summaryText}</Typography>
            <Typography>Recomendación: {result.recommendedNextStep}</Typography>
          </CardContent>
        </Card>
      )}
    </Stack>
  );
}
