import React, { useEffect, useState } from 'react';
import { JobStatus, TranslationJobDto, TranslatorDto } from '../api/clientApi';
import clientApi from '../api';

interface TranslationDetailProps {
  data: TranslationJobDto;
}

const TranslationDetail: React.FC<TranslationDetailProps> = ({ data }) => {
  const [translation, setTranslation] = useState(data.translatedContent ?? '');
  const [translators, setTranslators] = useState<TranslatorDto[]>([]);
  const [translator, setTranslator] = useState<TranslatorDto>();
  const [translatorId, setTranslatorId] = useState<string>();

  
  useEffect(() => {
    console.log(data);
    if (data.translatorId == null) loadTranslators();
    else loadTranslator()
  }, [data.id]);

  const loadTranslators = () => {
    clientApi.translator_GetAll().then(setTranslators);
  }

  const loadTranslator = () => {
    clientApi.translator_GetById(data.translatorId!).then(setTranslator);
  }

  const assignTranslator = () => {
    let item = translators.find(i => `${i.id}` === translatorId);
    console.log(item, translatorId)
    if (!item) return;
    clientApi.translator_AssignJob(item.id, data.id)
      .then(i => loadTranslator())
      .catch(i => console.error(i));
  }

  return (
    <>
      <div className="attributes">
        <div className="row">
          <span className="w-150">Id:</span>
          <span>{data.id}</span>
        </div>
        <div className="row">
          <span className="w-150">Customer Name:</span>
          <span>{data.customerName}</span>
        </div>
        <div className="row">
          <span className="w-150">Status:</span>
          <span>{JobStatus[data.status]}</span>
        </div>
        <div className="row">
          <span className="w-150">Price:</span>
          <span>{data.price}</span>
        </div>
        <div className="row">
          <span className="w-150">Original Contet:</span>
          <div className="row flex-1"><input type="string" className="w-100p" disabled value={data.originalContent} title='Original Content'/></div>
        </div>
        <div className="row">
          <span className="w-150">Translated Contet:</span>
          <div className="row flex-1">
            <div className="w-100p">
              <input type="string" className="w-100p" value={translation} title='Original Content' onChange={(e) => setTranslation(e.target.value)} />
            </div>
            <div className="w-150">
                <button onClick={() => {}} className="w-100p" >Update translation</button>
            </div>
          </div>
        </div>
        
        
        <div className="row">
          <span className="w-150">Translator:</span>
          {data.translatorId && translator && <span>{translator.name}</span>}
          
          {!data.translatorId &&
            <div className="row flex-1">
              <div className="w-100p">
                <select className="w-100p" title='Translators'  onChange={e => setTranslatorId(e.target.value)}>
                  <option id={`xxx`} value="xxx">Select item...</option>
                  {translators.map(i => 
                    <option key={i.id} value={i.id}>{i.name}</option>
                  )}
                </select>
              </div>
              <div className="w-150">
                  <button disabled={!translatorId} onClick={assignTranslator} className="w-100p">Assign translator</button>
              </div>
            </div>
          }
        </div>
      </div>
    </>
  );
};

export default TranslationDetail;
