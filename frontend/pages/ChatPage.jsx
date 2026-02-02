import { useState } from 'react';
import { Box, Button, Card, CardContent, Stack, TextField, Typography } from '@mui/material';
import api from '../services/api';

export default function ChatPage() {
  const [sessionId, setSessionId] = useState('');
  const [messages, setMessages] = useState([]);
  const [input, setInput] = useState('');

  const startSession = async () => {
    const { data } = await api.post('/chat/sessions', { patientId: null });
    setSessionId(data.sessionId);
    setMessages([]);
  };

  const sendMessage = async () => {
    if (!sessionId || !input) return;
    const { data } = await api.post(`/chat/sessions/${sessionId}/messages`, {
      sender: 'Patient',
      text: input,
    });
    const updated = await api.get(`/chat/sessions/${sessionId}/messages`);
    setMessages(updated.data);
    setInput('');
    return data;
  };

  return (
    <Stack spacing={3}>
      <Typography variant="h4">Chat de Telemedicina</Typography>
      <Button variant="contained" onClick={startSession}>
        Iniciar sesión
      </Button>
      {sessionId && (
        <Typography variant="subtitle1">Sesión activa: {sessionId}</Typography>
      )}
      <Card>
        <CardContent>
          <Stack spacing={2}>
            {messages.map((msg) => (
              <Box key={msg.messageId} sx={{ p: 1, backgroundColor: msg.sender === 'Bot' ? '#e3f2fd' : '#fff' }}>
                <Typography variant="caption">{msg.sender}</Typography>
                <Typography>{msg.text}</Typography>
              </Box>
            ))}
          </Stack>
        </CardContent>
      </Card>
      <Stack direction="row" spacing={2}>
        <TextField
          fullWidth
          label="Escribe tu mensaje"
          value={input}
          onChange={(event) => setInput(event.target.value)}
        />
        <Button variant="contained" onClick={sendMessage}>
          Enviar
        </Button>
      </Stack>
    </Stack>
  );
}
