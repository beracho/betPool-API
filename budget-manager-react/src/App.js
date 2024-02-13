import './App.css';
import { BrowserRouter, Routes, Route } from 'react-router-dom';
import SigninPage from './Pages/LoginPage';
import Dashboard from './Pages/Dashboard';

function App() {
  const activeSession = localStorage.getItem('@username');

  return (
    <div className="App">
      <BrowserRouter>
        <Routes>
          {!activeSession && <Route path="/login" element={<SigninPage />} />}
          {!activeSession && <Route path="/register" element={<SigninPage />} />}
          {!activeSession && <Route path="/recover" element={<SigninPage />} />}
          {!activeSession && <Route path="*" element={<SigninPage />} />}
          {activeSession && <Route path="/dashboard" element={<Dashboard />} />}
        </Routes>
      </BrowserRouter>
    </div>
  );
}

export default App;
