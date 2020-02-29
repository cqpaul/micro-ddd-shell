import React, { Component } from 'react'
import api from '../../api'
import './index.css'
import { Column, Table } from 'react-virtualized'
import 'react-virtualized/styles.css' // only needs to be imported once

export default class SummaryList extends Component {
  constructor(props) {
    super(props)

    this.state = {
      summaryList: []
    }
    this._onRowClick = this._onRowClick.bind(this)
  }

  componentDidMount() {}

  getSummaryListData(subwayName) {
    let getUrl = '/ReportingService/Reporting/GetAllVillageBaseInfos'
    if (subwayName !== undefined && subwayName !== 'undefined') {
      getUrl = `/ReportingService/Reporting/GetAllVillageBaseInfos?subwayName=${subwayName}`
    }
    api
      .callApi('GET', getUrl)
      .then(response => {
        this.setState({ summaryList: response })
      })
      .then(() => {
        this.props.finishedFetching()
      })
  }

  _onRowClick(event) {
    let villageName = event.rowData.villageName
    this.props.getFilteredPosition(villageName)
  }

  render() {
    return (
      <div className="summary-list-div">
        <Table
          width={700}
          height={780}
          headerHeight={20}
          rowHeight={30}
          rowCount={this.state.summaryList.length}
          rowGetter={({ index }) => this.state.summaryList[index]}
          onRowClick={this._onRowClick}
        >
          <Column width={220} label="小区名" dataKey="villageName" />
          <Column width={80} label="年代" dataKey="buildingYears" />
          <Column width={150} label="平均总价" dataKey="averageTotalPrice" />
          <Column width={150} label="平均单价" dataKey="averagePrice" />
          <Column width={150} label="最高房价" dataKey="topPrice" />
          <Column width={150} label="最低房价" dataKey="lowestPrice" />
          <Column width={80} label="套数" dataKey="sellCount" />
        </Table>
      </div>
    )
  }
}
