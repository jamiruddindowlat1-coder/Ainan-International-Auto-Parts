import React from 'react';
import { FiSearch, FiX } from 'react-icons/fi';

/**
 * SearchBar — Reusable search input component used across all ERP pages.
 *
 * @param {Object}   props
 * @param {string}   props.value         - Controlled input value
 * @param {Function} props.onChange      - onChange handler
 * @param {string}   [props.placeholder] - Placeholder text
 * @param {Function} [props.onClear]     - Optional clear button handler
 * @param {string}   [props.width]       - CSS width (default: '280px')
 */
export default function SearchBar({
  value = '',
  onChange,
  placeholder = 'Search...',
  onClear,
  width = '280px',
}) {
  const handleClear = () => {
    if (onClear) {
      onClear();
    } else if (onChange) {
      onChange({ target: { value: '' } });
    }
  };

  return (
    <div style={{ position: 'relative', width }}>
      <FiSearch
        style={{
          position: 'absolute',
          left: '12px',
          top: '50%',
          transform: 'translateY(-50%)',
          color: 'var(--text-muted)',
          fontSize: '1rem',
          pointerEvents: 'none',
        }}
      />
      <input
        type="text"
        className="form-control"
        value={value}
        onChange={onChange}
        placeholder={placeholder}
        style={{ paddingLeft: '36px', paddingRight: value ? '36px' : '12px' }}
      />
      {value && (
        <button
          type="button"
          onClick={handleClear}
          style={{
            position: 'absolute',
            right: '10px',
            top: '50%',
            transform: 'translateY(-50%)',
            background: 'none',
            border: 'none',
            color: 'var(--text-muted)',
            cursor: 'pointer',
            display: 'flex',
            alignItems: 'center',
            padding: '2px',
            borderRadius: '4px',
          }}
          title="Clear search"
        >
          <FiX fontSize={14} />
        </button>
      )}
    </div>
  );
}
