import { Link } from 'react-router-dom';
import logo from '../Assets/budgetManagerLogo.png';
import '../Styles/Sidebar.css';
import { FontAwesomeIcon } from '@fortawesome/react-fontawesome'

const Sidebar = () => {
  return (
    <div>
      <div>
        <img className="img-logo" src={logo} alt="Logo" />
      </div>
      <div className="menu-container">
        <h2 className="menu-title">
          Dashboard
        </h2>
        <Link to="/dashboard" className="nav-link">
          <span className="capitalize ">Dashboard</span>
        </Link>
        <Link to="/account" className="nav-link">
          <span className="capitalize ">Accounts Page</span>
        </Link>
      </div>
    </div>
  );
}

export default Sidebar;