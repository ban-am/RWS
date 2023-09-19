import React, { useState } from 'react';
import clientApi from '../api';

interface TranslationFormProps {
  onJobAdded: () => void;
}

const TranslationForm: React.FC<TranslationFormProps>  = ({ onJobAdded }) => {
  const [customerName, setCustomerName] = useState('');
  const [toTranslate, setToTranslate] = useState('');

  const handleAddTranslation = () => {
    if (customerName.trim() === '' || toTranslate.trim() === '') {
      alert('Please fill in both fields');
      return;
    }

    clientApi.translationJob_Create(customerName, toTranslate).then(() => {
      onJobAdded();
      setCustomerName('');
      setToTranslate('');
    });
  };

  return (
    <>
    <div className="attributes">
      <div className="row">
        <label htmlFor="customerName" className="w-150">Customer:</label>
        <div className="w-100p">
          <input type="string" className="w-100p" id="customerName" required  value={customerName} onChange={(e) => setCustomerName(e.target.value)} />
        </div>
      </div>
      <div className="row">
        <label htmlFor="toTranslate" className="w-150">To translate:</label>
        <div className="w-100p">
        <input type="string" className="w-100p" id="toTranslate" required value={toTranslate} onChange={(e) => setToTranslate(e.target.value)} />
      </div>
      </div>
    </div>

    <button disabled={!customerName || !toTranslate}  onClick={handleAddTranslation}>Create Job</button>
</>
  );
};

export default TranslationForm;