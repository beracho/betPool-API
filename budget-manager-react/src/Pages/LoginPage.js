import "../Styles/SigninPage.css"
import { useState, useEffect } from 'react';
import Recover from '../Components/Recover';
import Register from "../Components/Register";
import Signin from "../Components/Signin";
import { useLocation } from 'react-router-dom';

const SigninPage = () => {
    const [username, setUsername] = useState();
    const [password, setPassword] = useState();

    const location = useLocation();
    const { pathname } = location;

    const handleSubmit = (e) => {
        e.preventDefault();
    }

    useEffect(() => {
        console.log("use effect ran");
    }, []);

    return (
        <div className="body-login">
            <div className="center-login">
                <div className="card-login">
                    <div className="user-links">
                        <span>
                            <a href="/">Sign In</a>
                            <a>/</a>
                            <a href="/register">Sign up</a>
                        </span>
                    </div>
                    {pathname == "/" ? <Signin /> : pathname == "/register" ? <Register /> : <Recover />}
                </div>
            </div>
        </div>
    );
}

export default SigninPage;