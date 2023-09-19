import React from 'react';

interface Props {
  children: React.ReactNode
}

const Layout: React.FC<Props> = (props: Props) =>  {
  
  return <>
    <div className="App">
        {props.children}
    </div>
  </>;
}

export default Layout;