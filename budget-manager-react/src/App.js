import './App.css';
import { BrowserRouter, Routes, Route } from 'react-router-dom';
import SigninPage from './Pages/LoginPage';

function App() {
  return (
    <div className="App">
      <BrowserRouter>
        <Routes>
          {/* <Route path="/" exact element={<Dashboard />} /> */}
          <Route path="/" element={<SigninPage />} />
          <Route path="/register" element={<SigninPage />} />
          <Route path="/recover" element={<SigninPage />} />
        </Routes>
      </BrowserRouter>
    </div>
  );
}

export default App;
