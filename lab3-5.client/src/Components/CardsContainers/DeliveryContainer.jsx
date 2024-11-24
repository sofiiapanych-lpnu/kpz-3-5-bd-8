import {React} from 'react';
import DeliveryCard from '../Cards/deliveryCard';

const DeliveryContainer = ({ deliveries, isLoading, error }) => {
  if (isLoading) {
    return <p>Loading...</p>;
  }

  if (error) {
    return <p>{error}</p>;
  }

  return (
    <div className="row">
      {deliveries.map(delivery => (
        <div className="col-12" key={delivery.deliveryId}>
          <DeliveryCard delivery={delivery} />
        </div>
      ))}
    </div>
  );
};

export default DeliveryContainer;
