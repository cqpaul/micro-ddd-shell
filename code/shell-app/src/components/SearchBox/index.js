import React, { Component } from 'react'
import { Select } from 'antd'
import './index.css'
const { Option } = Select

export default class SearchBox extends Component {
  constructor(props) {
    super(props)
    this.handleChange = this.handleChange.bind(this)
  }

  handleChange = value => {
    this.props.handleVillageSelect(value)
  }
  handleSearch = value => {
    this.props.handleVillageSearch(value)
  }

  render() {
    const options = this.props.villageNameList.map(d => (
      <Option key={d}>{d}</Option>
    ))
    return (
      <div>
        <Select
          showSearch
          allowClear={true}
          // defaultValue="1号线"
          placeholder="选择地铁线"
          onChange={this.handleChange}
          onSearch={this.handleSearch}
        >
          {options}
        </Select>
      </div>
    )
  }
}
