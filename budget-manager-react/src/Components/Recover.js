import "../Styles/SigninPage.css"
import { useState } from 'react';
import { Link } from 'react-router-dom';

const Recover = () => {
    const [email, setEmail] = useState();

    const handleSubmit = (e) => {
        e.preventDefault();
    }

    return (
        <form className="login-form" onSubmit={handleSubmit}>
            <h1>RECOVER PASSWORD</h1>
            <div className="form-group">
                <label htmlFor="">Email</label>
                <input
                    className="form-field"
                    type="text"
                    required
                    value={email}
                    onChange={(e) => setEmail(e.target.value)}
                />
            </div>
            <div className="login-link">
                <Link to="/login">Back to login</Link>
            </div>

            <button className="button">SEND EMAIL</button>
        </form>
    );
}

export default Recover;