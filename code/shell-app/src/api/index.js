import axios from 'axios'
import '@babel/polyfill'
// import router from '../router/index'
import { message } from 'antd'

const rootUrl = 'http://localhost:5000'
const singalRUrl = 'http://localhost:5003'

// #region Tool Functions
const toType = obj => {
  return {}.toString
    .call(obj)
    .match(/\s([a-zA-Z]+)/)[1]
    .toLowerCase()
}

const filterNull = o => {
  for (let key in o) {
    if (o.hasOwnProperty(key)) {
      if (o[key] === null) {
        delete o[key]
      }
      if (toType(o[key]) === 'string') {
        // o[key] = o[key]
      } else if (toType(o[key]) === 'object') {
        o[key] = filterNull(o[key])
      } else if (toType(o[key]) === 'array') {
        o[key] = filterNull(o[key])
      }
    }
  }
  return o
}

const checkToken = () => {
  // let token = window.localStorage.getItem('token')
  // if (!token) {
  //   // router.push('/login')
  //   return false
  // }
  return true
}
// #endregion

function apiAxios(method, url, params, success, failure) {
  if (params) {
    params = filterNull(params)
  }
  let opts = {
    method: method,
    url: url,
    data: method === 'POST' || method === 'PUT' ? params : null,
    params: method === 'GET' || method === 'DELETE' ? params : null,
    baseURL: rootUrl,
    timeout: 200000,
    headers: {
      'Access-Control-Allow-Origin': '*',
      'Access-Control-Allow-Methods': '*',
      'Access-Control-Allow-Headers': '*',
      Accept: 'application/json',
      'Content-Type': 'application/json',
      Authorization: `Bearer ${window.localStorage.getItem('token')}`
    },
    withCredentials: false
  }
  if (checkToken()) {
    // checkToken()
    axios(opts)
      .then(function(res) {
        if (res.status === 200) {
          if (success) {
            success(res.data)
          }
        } else {
          if (failure) {
            failure(res.data)
          } else {
            console.log('error: ' + JSON.stringify(res.data.message))
          }
        }
      })
      .catch(function(err) {
        if (err.toString().indexOf('401') !== -1) {
          window.location.href = process.env.VUE_APP_MVCURL
        }
        if (err.toString().indexOf('403') !== -1) {
          message.error('401')
        }
        if (err) {
          console.log('api error, HTTP CODE: ' + err)
        }
      })
  }
}

function exportExcel(url, options = {}, fileBaseName) {
  return new Promise((resolve, reject) => {
    axios({
      method: 'post',
      url: url,
      data: options,
      responseType: 'blob',
      baseURL: rootUrl,
      timeout: 200000,
      headers: {
        'Access-Control-Allow-Origin': '*',
        'Access-Control-Allow-Methods': '*',
        'Access-Control-Allow-Headers': '*',
        Accept: 'application/json',
        'Content-Type': 'application/json',
        Authorization: `Bearer ${window.localStorage.getItem('token')}`
      },
      withCredentials: false
    }).then(response => {
      resolve(response.data)
      let blob = new Blob([response.data], { type: 'application/vnd.ms-excel' })
      let fileName = fileBaseName + '.xlsx'
      if (window.navigator.msSaveOrOpenBlob) {
        navigator.msSaveBlob(blob, fileName)
      } else {
        var link = document.createElement('a')
        link.href = window.URL.createObjectURL(blob)
        link.download = fileName
        link.click()
        window.URL.revokeObjectURL(link.href)
      }
    })
  })
}

export default {
  callApi(method, url, params) {
    return new Promise((resolve, reject) => {
      apiAxios(method, url, params, resolve, reject)
    })
  },
  get(url, params, success, failure) {
    return apiAxios('GET', url, params, success, failure)
  },
  post(url, params, success, failure) {
    return apiAxios('POST', url, params, success, failure)
  },
  put(url, params, success, failure) {
    return apiAxios('PUT', url, params, success, failure)
  },
  delete(url, params, success, failure) {
    return apiAxios('DELETE', url, params, success, failure)
  },
  rootUrl,
  singalRUrl,
  exportExcel
}
