import { Link } from "react-router-dom";
import { useContext, useState, useEffect } from "react";
import "./Header.css";
import axios from "axios";

const Header = () => {
  const handleCommit = async () => {
    try {
      const response = await axios.post('/api/Delivery/commit'); 
      alert(`Commit : ${response.data}`); 
      console.log(response)
    } catch (error) {
      console.error('Commit failed:', error);
      alert('Commit failed. Please try again.');
    }
  };

  return (
    <header className="header-container container-fluid p-3 fixed-top">
      {/* className="d-flex align-items-center me-5" */}
      <Link to="/" > 
        logo
      </Link>
      <button className="dropdown-toggle" type="button" data-bs-toggle="dropdown" aria-expanded="false">Tables</button>
      <ul className="dropdown-menu">
        <li><Link className="dropdown-item" to="/person">Person</Link></li>
        <li><Link className="dropdown-item" to="/delivery">Delivery</Link></li>
      </ul>
      <button className="btn-commit ms-auto" onClick={handleCommit}>
        Commit
      </button>
    </header>
  );
};

export default Header;
