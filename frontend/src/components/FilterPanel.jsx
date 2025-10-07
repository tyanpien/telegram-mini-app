import React from 'react';
import './FilterPanel.css';

const FilterPanel = ({ filters, onFilterChange, products }) => {
  const uniqueSteelGrades = [...new Set(products.map(p => p.steelGrade).filter(Boolean))];
  const uniqueDiameters = [...new Set(products.map(p => p.diameter).filter(Boolean))];
  const uniqueWallThickness = [...new Set(products.map(p => p.pipeWallThickness).filter(Boolean))];
  const uniqueManufacturers = [...new Set(products.map(p => p.manufacturer).filter(Boolean))];

  const handleFilterChange = (key, value) => {
    onFilterChange({
      ...filters,
      [key]: value
    });
  };

  const clearFilters = () => {
    onFilterChange({
      steelGrade: '',
      diameter: '',
      wallThickness: '',
      manufacturer: ''
    });
  };

  return (
    <div className="filter-panel">
      <div className="filter-group">
        <label>Марка стали:</label>
        <select
          value={filters.steelGrade}
          onChange={(e) => handleFilterChange('steelGrade', e.target.value)}
        >
          <option value="">Все</option>
          {uniqueSteelGrades.map(grade => (
            <option key={grade} value={grade}>{grade}</option>
          ))}
        </select>
      </div>

      <div className="filter-group">
        <label>Диаметр:</label>
        <select
          value={filters.diameter}
          onChange={(e) => handleFilterChange('diameter', e.target.value)}
        >
          <option value="">Все</option>
          {uniqueDiameters.map(diameter => (
            <option key={diameter} value={diameter}>{diameter} мм</option>
          ))}
        </select>
      </div>

      <div className="filter-group">
        <label>Толщина стенки:</label>
        <select
          value={filters.wallThickness}
          onChange={(e) => handleFilterChange('wallThickness', e.target.value)}
        >
          <option value="">Все</option>
          {uniqueWallThickness.map(thickness => (
            <option key={thickness} value={thickness}>{thickness} мм</option>
          ))}
        </select>
      </div>

      <div className="filter-group">
        <label>Производитель:</label>
        <select
          value={filters.manufacturer}
          onChange={(e) => handleFilterChange('manufacturer', e.target.value)}
        >
          <option value="">Все</option>
          {uniqueManufacturers.map(manufacturer => (
            <option key={manufacturer} value={manufacturer}>{manufacturer}</option>
          ))}
        </select>
      </div>

      <button className="clear-filters" onClick={clearFilters}>
        Очистить фильтры
      </button>
    </div>
  );
};

export default FilterPanel;
