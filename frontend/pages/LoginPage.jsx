import { useState } from 'react';
import { Button, Card, CardContent, Stack, TextField, Typography } from '@mui/material';
import { login } from '../auth/authService';

export default function LoginPage() {
  const [form, setForm] = useState({ email: '', password: '' });
  const [result, setResult] = useState(null);

  const submit = async () => {
    const data = await login(form);
    setResult(data);
  };

  return (
    <Stack spacing={3}>
      <Typography variant="h4">Login</Typography>
      <Card>
        <CardContent>
          <Stack spacing={2}>
            <TextField label="Email" value={form.email} onChange={(e) => setForm({ ...form, email: e.target.value })} />
            <TextField label="Password" type="password" value={form.password} onChange={(e) => setForm({ ...form, password: e.target.value })} />
            <Button variant="contained" onClick={submit}>Ingresar</Button>
          </Stack>
        </CardContent>
      </Card>
      {result && <Typography>Token recibido para {result.role}</Typography>}
    </Stack>
  );
}
