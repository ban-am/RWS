import React, {useEffect, useState} from 'react';
import './App.css';
import clientApi from './api';
import { TranslationJobDto, TranslatorDto } from './api/clientApi';
import {TranslationForm, TranslationTable, TranslationDetail, TranslatorTable, TranslatorDetail, TranslatorForm} from './components';

export default function App() {
  const [jobs, setJobs] = useState<TranslationJobDto[]>([]);
  const [translators, setTranslators] = useState<TranslatorDto[]>([]);
  const [job, setJob] = useState<TranslationJobDto>();
  const [translator, setTranslator] = useState<TranslatorDto>();

  useEffect(() => {
    loadJobs();
    loadTranslators();
  }, []);

  const loadJobs = () => {
    return clientApi.translationJob_GetAll().then(i => setJobs(i));
  }

  const loadTranslators = () => {
    return clientApi.translator_GetAll().then(i => setTranslators(i));
  }



  return (
    <div>
      <div className='section'>
        <div className='section-header'>Jobs:</div>
        <TranslationTable data={jobs} onRowSelected={i => setJob(i)}/>
      </div>

      {job && 
        <div className='section'>
        <div className='section-header'>Job detail:</div>
          <TranslationDetail data={job}/>
        </div>
      }

      <div className='section'>
        <div className='section-header'>Job form:</div>
        <TranslationForm onJobAdded={loadJobs}/>
      </div>

      <div className='section'>
        <div className='section-header'>Translators:</div>
        <TranslatorTable data={translators} onRowSelected={i => setTranslator(i)}/>
      </div>
      
      {translator && 
        <div className='section'>
          <div className='section-header'>Translator detail:</div>
          <TranslatorDetail data={translator} onDeleted={() => {
            setTranslator(undefined);
            loadTranslators();
          }}/>
        </div>
      }

      <div className='section'>
        <div className='section-header'>Translator form:</div>
        <TranslatorForm onAdded={loadTranslators}/>
      </div>

    </div>
  )
}
