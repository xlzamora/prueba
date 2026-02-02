import { useState } from 'react';
import { Button, Card, CardContent, Stack, TextField, Typography } from '@mui/material';
import { registerPatient } from '../auth/authService';

export default function RegisterPage() {
  const [form, setForm] = useState({
    email: '',
    password: '',
    fullName: '',
    documentId: '',
    birthDate: '',
  });
  const [result, setResult] = useState(null);

  const submit = async () => {
    const data = await registerPatient({
      email: form.email,
      password: form.password,
      fullName: form.fullName,
      documentId: form.documentId,
      birthDate: form.birthDate,
    });
    setResult(data);
  };

  return (
    <Stack spacing={3}>
      <Typography variant="h4">Registro Paciente</Typography>
      <Card>
        <CardContent>
          <Stack spacing={2}>
            <TextField label="Email" value={form.email} onChange={(e) => setForm({ ...form, email: e.target.value })} />
            <TextField label="Password" type="password" value={form.password} onChange={(e) => setForm({ ...form, password: e.target.value })} />
            <TextField label="Nombre completo" value={form.fullName} onChange={(e) => setForm({ ...form, fullName: e.target.value })} />
            <TextField label="Documento" value={form.documentId} onChange={(e) => setForm({ ...form, documentId: e.target.value })} />
            <TextField label="Fecha de nacimiento (YYYY-MM-DD)" value={form.birthDate} onChange={(e) => setForm({ ...form, birthDate: e.target.value })} />
            <Button variant="contained" onClick={submit}>Registrar</Button>
          </Stack>
        </CardContent>
      </Card>
      {result && <Typography>Paciente creado: {result.userId}</Typography>}
    </Stack>
  );
}
