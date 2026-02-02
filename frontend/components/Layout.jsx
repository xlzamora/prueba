import { AppBar, Box, Button, Container, Toolbar, Typography } from '@mui/material';
import { Outlet, useNavigate } from 'react-router-dom';

export default function Layout() {
  const navigate = useNavigate();

  return (
    <Box sx={{ minHeight: '100vh', backgroundColor: '#f4f6f8' }}>
      <AppBar position="static">
        <Toolbar>
          <Typography variant="h6" sx={{ flexGrow: 1 }}>
            Telemedicina Odonto
          </Typography>
          <Button color="inherit" onClick={() => navigate('/')}>Chat</Button>
          <Button color="inherit" onClick={() => navigate('/triage')}>Triage</Button>
          <Button color="inherit" onClick={() => navigate('/medical-profile')}>Ficha Médica</Button>
          <Button color="inherit" onClick={() => navigate('/appointments')}>Agenda</Button>
          <Button color="inherit" onClick={() => navigate('/dentist')}>Odontólogo</Button>
          <Button color="inherit" onClick={() => navigate('/admin/kb')}>Admin KB</Button>
        </Toolbar>
      </AppBar>
      <Container sx={{ py: 4 }}>
        <Outlet />
      </Container>
    </Box>
  );
}
