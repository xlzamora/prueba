import { useState } from 'react';
import { Button, Card, CardContent, Stack, TextField, Typography } from '@mui/material';
import api from '../services/api';

export default function AdminKbPage() {
  const [items, setItems] = useState([]);
  const [form, setForm] = useState({
    category: 'FAQ',
    title: '',
    content: '',
    tagsJson: '[]',
    isActive: true,
  });

  const load = async () => {
    const { data } = await api.get('/admin/kb');
    setItems(data);
  };

  const create = async () => {
    const { data } = await api.post('/admin/kb', form);
    setItems((prev) => [data, ...prev]);
  };

  return (
    <Stack spacing={3}>
      <Typography variant="h4">Admin Knowledge Base</Typography>
      <Button variant="contained" onClick={load}>Cargar KB</Button>
      <Card>
        <CardContent>
          <Stack spacing={2}>
            <TextField label="Categoría" value={form.category} onChange={(e) => setForm({ ...form, category: e.target.value })} />
            <TextField label="Título" value={form.title} onChange={(e) => setForm({ ...form, title: e.target.value })} />
            <TextField label="Contenido" value={form.content} onChange={(e) => setForm({ ...form, content: e.target.value })} multiline rows={3} />
            <TextField label="Tags JSON" value={form.tagsJson} onChange={(e) => setForm({ ...form, tagsJson: e.target.value })} />
            <Button variant="outlined" onClick={create}>Crear</Button>
          </Stack>
        </CardContent>
      </Card>
      {items.map((item) => (
        <Card key={item.kbId}>
          <CardContent>
            <Typography variant="h6">{item.title}</Typography>
            <Typography>{item.content}</Typography>
          </CardContent>
        </Card>
      ))}
    </Stack>
  );
}
