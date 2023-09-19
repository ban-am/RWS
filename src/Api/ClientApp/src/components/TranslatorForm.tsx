import React, { useState } from 'react';
import clientApi from '../api';

interface TranslatorFormProps {
  onAdded: () => void;
}

const TranslatorForm: React.FC<TranslatorFormProps>  = ({ onAdded: onJobAdded }) => {
  const [name, setName] = useState('');

  const onAdd = () => {
    if (name.trim() === '') {
      alert('Please fill name field');
      return;
    }

    clientApi.translator_Add({
      name: name,
      hourlyRate: 100,
      creditCardNumber: "123-321-456-987"
    }).then(() => {
      onJobAdded();
      setName('');
    });
  };

  return (
    <>
    <div className="attributes">
      <div className="row">
        <label htmlFor="translatorName" className="w-150">Name:</label>
        <div className="w-100p">
          <input type="string" className="w-100p" id="translatorName" required  value={name} onChange={(e) => setName(e.target.value)} />
        </div>
      </div>
    </div>

    <button disabled={!name}  onClick={onAdd}>Create Translator</button>
</>
  );
};

export default TranslatorForm;