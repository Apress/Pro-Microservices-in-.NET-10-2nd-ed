import { useState } from 'react';
import { trace } from '@opentelemetry/api';

const tracer = trace.getTracer('react-components');

function Counter() {
  const [count, setCount] = useState(0);

  function handleClick() {
    const span = tracer.startSpan('Counter.handleClick');
    setCount((prevCount) => {
      const newCount = prevCount + 1;
      span.setAttribute('count', newCount);
      span.end();
      return newCount;
    });
  }

  return (
    <div>
      <button onClick={handleClick}>
        count is {count}
      </button>
    </div>
  );
}

export default Counter;
