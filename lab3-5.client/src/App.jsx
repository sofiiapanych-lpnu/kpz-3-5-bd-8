import { useEffect, useState } from 'react';
import { BrowserRouter as Router, Route, Routes } from 'react-router-dom';
import './App.css';
import Header from './Components/Header/Header';
import HomePage from './Pages/Home/HomePage';
import Person from './Pages/Person/Person'
import Delivery from './Pages/Delivery/Delivery'
import DeliveryFullCard from './Components/Cards/DeliveryFullCard';
import PersonFullCard from './Components/Cards/PersonFullCard'

function App() {
    return (
        <Router>
            <Header />
            <Routes>
                <Route path="/" element={<HomePage />} />
                <Route path="/person" element={<Person />} />
                <Route path="/person/:personId" element={<PersonFullCard />} />
                <Route path="/delivery" element={<Delivery />} />
                <Route path="/delivery/:deliveryId" element={<DeliveryFullCard />} />
            </Routes>
        </Router>
    );
}

export default App;