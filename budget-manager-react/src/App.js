import './App.css';
import { BrowserRouter, Routes, Route } from 'react-router-dom';
import SigninPage from './Pages/LoginPage';
import Dashboard from './Pages/Dashboard';
import Sidebar from './Components/Sidebar';
import Navbar from './Components/Navbar';
import AccountPage from './Pages/AccountPage';

function App() {
  const activeSession = localStorage.getItem('@username');

  return (
    <div className="App">
      <BrowserRouter>
        {!activeSession ? (
          <Routes>
            <Route path="/login" element={<SigninPage />} />
            <Route path="/register" element={<SigninPage />} />
            <Route path="/recover" element={<SigninPage />} />
            <Route path="/resetPassword/:code" element={<SigninPage />} />
            <Route path="*" element={<SigninPage />} />
          </Routes>
        ) : null}
        {activeSession ? (
          <div className="main-container">
            <div className="sidebar-container">
              <Sidebar />
            </div>
            <div className="right-side-container">
              <div className="navbar-container">
                <Navbar />
              </div>
              <div className="content-container">
                <Routes>
                  {/* dashboard  */}
                  <Route path="/dashboard" element={<Dashboard />} />
                  <Route path="/account" element={<AccountPage />} />
                  <Route path="*" element={<Dashboard />} />
                </Routes>
              </div>
            </div>
          </div>
        ) : null}
      </BrowserRouter>
    </div>
  );
}

export default App;
