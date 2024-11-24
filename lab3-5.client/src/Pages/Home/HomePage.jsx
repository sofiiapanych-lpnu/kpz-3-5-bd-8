import React from 'react';
import { Link } from 'react-router-dom';
import './HomePage.css';

const HomePage = () => {
    return (
        <div className="home-page">
            <main className="content">
                <section className="intro text-center mb-5">
                    <h2>Ласкаво просимо до системи кур'єрської доставки</h2>
                    <p>бєбєбє</p>
                </section>
    
                <Link to="/" className="section-link text-decoration-none">
                    <section className="configuration text-center mb-4">
                        <h3>Конфігурація будівлі</h3>
                        <p>Визначте кількість поверхів, площу приміщень та додайте охоронні датчики.</p>
                    </section>
                </Link>

                <Link to="/" className="section-link text-decoration-none">
                    <section className="simulation text-center">
                        <h3>Запуск симуляції</h3>
                        <p>Натисніть, щоб розпочати симуляцію з обраними налаштуваннями.</p>
                    </section>
                </Link>
            </main>
    

        </div>
    );
}

export default HomePage;
